import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { OktaAuthService } from '@okta/okta-angular';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';
import { Card } from '../card/card';
import { Phase } from '../phase/phase';
import { CardCombination } from '../phase/cardCombination';
import { PlayerResourceModel } from '../player/playerResourceModel';
import { Stage } from './stage';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ScoreCardResourceModel } from '../player/ScoreCardResourceModel';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ScoreCardComponent } from '../score-card/score-card.component';
import { CardType } from '../card/CardType';
import { MatDialog } from '@angular/material/dialog';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';


@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  gameConnection: HubConnection;
  gameId: string;
  discardPile: Card;
  deckCard: Card;
  activePlayer: string;
  player: PlayerResourceModel;
  isValidPhase: boolean;
  competitors: PlayerResourceModel[];

  viewColumns: number = 12;
  viewRows: number = 5;

  discardPileId: string = 'discardPileDropList';
  playerHandId: string = 'playerHandDropList';
  deckId: string = 'deckDropList';
  playerComboOneId: string = 'playerComboOneDropList';
  playerComboTwoId: string = 'playerComboTwoDropList';

  constructor(public oktaAuth: OktaAuthService, private route: ActivatedRoute, private snackBar: MatSnackBar, private bottomSheet: MatBottomSheet, private dialog: MatDialog) {
    this.player = {
      hand: [] as Card[],
      phase: {
        cardCombinations: [] as CardCombination[]
      } as Phase
    } as PlayerResourceModel;
    this.discardPile = {} as Card;
    this.deckCard = {
      cardColor: -1,
      cardType: CardType.Deck
    }
    this.isValidPhase = false;
    this.competitors = [] as PlayerResourceModel[];
   }

  ngOnInit(): void {
    this.gameId = this.route.snapshot.paramMap.get('id');
    this.establishGameConnection();
  }

  async onDiscardPileDropped(card: Card) {
    if(this.player.id !== this.activePlayer) {
      this.snackBar.open("It's not your turn motherfucker!", null, {
        duration: 2000
      });
    }
    else if (this.player.stage !== Stage.Discard) {
      this.snackBar.open("You must draw a card first. Duh.", null, {
        duration: 2000
      });
    }
    else {
      let index = this.player.hand.indexOf(card);
      if(index > -1) {
        this.player.hand.splice(index, 1);
        this.sortHand();
        this.discardPile = {...card};

        this.gameConnection.send("SetDiscardPile", this.gameId, this.player.id, card);
      }
    }
    
  }

  onHandDropped(dropData: CdkDragDrop<Card | Card[], Card | Card[]>) {
    
    const previousId = dropData.previousContainer.id;
    const currentId = dropData.container.id;

    // Are they just moving cards in their hand?
    if (previousId === currentId) {
      this.handleHandToHandDrop(dropData.container.data as Card[], dropData.previousIndex, dropData.currentIndex);
    }
    else if (previousId === this.playerComboOneId || previousId === this.playerComboTwoId) {
      // Dropping onto our hand from a phase

      this.handlePhaseToHandDrop(<Card[]>dropData.previousContainer.data,
                                <Card[]>dropData.container.data,
                                dropData.previousIndex,
                                dropData.currentIndex);
      
    } else { // Or from the deck or discard pile?
      const isFromDiscardPile: boolean = dropData.previousContainer.id === this.discardPileId;

      if (isFromDiscardPile) {
        this.handleDiscardToHandDrop(dropData.currentIndex);
      } else {
        this.handleDeckToHandDrop(dropData.currentIndex);
      }

    }
  }

  handleHandToHandDrop = (hand: Card[], previousIndex: number, currentIndex: number) => {
    moveItemInArray(hand, previousIndex, currentIndex);
  }

  handlePhaseToHandDrop = (combo: Card[], hand: Card[], previousIndex: number, currentIndex: number) => {
    if (this.player.hasCompletedPhase) {
      this.snackBar.open("You can't remove a card from a completed phase", null, {
        duration: 2000
      });

    } else {
      transferArrayItem(combo,
                        hand,
                        previousIndex,
                        currentIndex);
    }
  }

  handleDiscardToHandDrop = (currentIndex: number) => {
    if(this.player.id !== this.activePlayer) {
      this.snackBar.open("Seriously?! It's not your turn. Mother. Fucker.", null, {
        duration: 2000
      });
    }
    else if (this.player.stage !== Stage.Draw) {
      this.snackBar.open("You must discard a card", null, {
        duration: 2000
      });
    }
    else {
      this.gameConnection.invoke("GetDiscardPile", this.gameId, this.player.id)
          .then((card: Card) => {
            this.player.hand.splice(currentIndex, 0, card);
            this.player.stage = Stage.Discard;
          });
    }
  }

  handleDeckToHandDrop = (currentIndex: number) => {
    if(this.player.id !== this.activePlayer) {
      this.snackBar.open("It's not your turn", null, {
        duration: 2000
      });
    }
    else if (this.player.stage !== Stage.Draw) {
      this.snackBar.open("You must discard a card", null, {
        duration: 2000
      });
    }
    else {
      this.gameConnection.invoke("GetTopCard", this.gameId, this.player.id)
        .then((card: Card) => {
          this.player.hand.splice(currentIndex, 0, card);
          this.player.stage = Stage.Discard;
        });
    }
  }

  onPlayerComboDropped = (dropData: CdkDragDrop<Card[], Card[]>) => {
    console.log('combo dropped', dropData);
    if (dropData.previousContainer.id === this.playerHandId) {
      this.handleHandToComboDrop(dropData.previousContainer.data, dropData.container.data, dropData.previousIndex, dropData.currentIndex);
    }
    else if (dropData.previousContainer.id === dropData.container.id) {
      this.handleComboToSameComboDrop(dropData.container.data, dropData.previousIndex, dropData.currentIndex);
    }
    else {
      this.handleComboToDifferentComboDrop(dropData.previousContainer.data, dropData.container.data, dropData.previousIndex, dropData.currentIndex);
    }
  }

  handleHandToComboDrop = (hand: Card[], combo: Card[], previousIndex: number, currentIndex: number) => {
    if (this.player.hasCompletedPhase) {
      this.tryAddToPhase(hand, this.player, combo, previousIndex, currentIndex);
    } else {
      transferArrayItem(hand, combo, previousIndex, currentIndex);
      this.tryPlayPhase();
    }
  }

  handleComboToSameComboDrop(combo: Card[], previousIndex: number, currentIndex: number) {
    
    if (this.player.hasCompletedPhase) {
      this.snackBar.open("You can't move cards in a completed combination", null, {
        duration: 2000
      });
    } else {
      moveItemInArray(combo, previousIndex, currentIndex);
      this.tryPlayPhase();
    }
  }

  handleComboToDifferentComboDrop = (first: Card[], second: Card[], previousIndex: number, currentIndex: number) => {
    if(this.player.hasCompletedPhase) {
      this.snackBar.open("You can't move cards in a completed combination", null, {
        duration: 2000
      });
    } else {
      transferArrayItem(first, second, previousIndex, currentIndex);
    }
  }

  onCompetitorComboDropped = (competitor: PlayerResourceModel, dropData: CdkDragDrop<Card[], Card[]>) => {
    console.log('player phase', dropData);
    if (!this.player.hasCompletedPhase) {
      this.snackBar.open("You must complete your own phase first", null, {duration: 2000});
    } else {
      this.handleHandToCompetitorComboDrop(dropData.previousContainer.data, competitor, dropData.container.data, dropData.previousIndex, dropData.currentIndex);
    }
  }

  handleHandToCompetitorComboDrop = (hand: Card[], competitor: PlayerResourceModel, combo: Card[], previousIndex: number, currentIndex: number) => {
    if (!competitor.hasCompletedPhase) {
      this.snackBar.open("You must wait for your competitor to play their phase", null, {duration: 2000});
    } else {
      this.tryAddToPhase(hand, competitor, combo, previousIndex, currentIndex);
    }
  }

  tryPlayPhase() {
    this.gameConnection.invoke("CheckPhase", this.player.phase)
    .then((isValidPhase: boolean) => {
      this.isValidPhase = isValidPhase;

      if (this.isValidPhase) {
        this.gameConnection.send("PlayPhase", this.gameId, this.player.id, this.player.phase)
        .then(() => {
          this.snackBar.open("You played your phase!", null, {
            duration: 2000
          });
          this.player.hasCompletedPhase = true;
        });
      } 
    });
  }

  tryAddToPhase(hand: Card[], player: PlayerResourceModel, combo: Card[], previousIndex: number, currentIndex: number) {
    let isAtStartOrEnd: boolean = currentIndex === 0 || currentIndex === combo.length - 1;

    if (isAtStartOrEnd) {
      transferArrayItem(hand, combo, previousIndex, currentIndex);
      this.gameConnection.invoke('CheckPhase', player.phase)
        .then((isValidPhase: boolean) => {
          if (isValidPhase) {
            this.gameConnection.send("UpdateHand", this.gameId, player.id, hand);
            this.gameConnection.send("UpdatePhase", this.gameId, player.id, player.phase);
          } else {
            this.snackBar.open('Not a valid combo', null, {duration: 2000});
            transferArrayItem(combo, hand, currentIndex, previousIndex);
          }
        });
    } else {
      this.snackBar.open("You can't play in the middle of a completed phase", null, {
        duration: 2000
      });
    }
  }

  activePlayerName(): string {
    return this.player.id === this.activePlayer ? this.player.name : this.competitors.find(x => x.id === this.activePlayer).name;
  }

  async selectDeck() {
    const userClaims = await this.oktaAuth.getUser();
    const playerId = userClaims.email;

    if(this.player.id !== this.activePlayer) {
      this.snackBar.open("It's not your turn", null, {
        duration: 2000
      });
    }
    else if (this.player.stage !== Stage.Draw) {
      this.snackBar.open("You must discard a card", null, {
        duration: 2000
      });
    }
    else {
      this.gameConnection.invoke("GetTopCard", this.gameId, this.player.id)
        .then((card: Card) => {
          this.player.hand.push(card);
          this.sortHand();
          this.player.stage = Stage.Discard;
        });
    }
  }

  sortHand() {
    this.player.hand.sort((a, b) => {
      if (a.cardType > b.cardType) return 1;
      if (a.cardType < b.cardType) return -1;

      if (a.cardColor > b.cardColor) return 1;
      if (a.cardColor < b.cardColor) return -1;
    });
  }

  async establishGameConnection() {
    const userClaims = await this.oktaAuth.getUser();
    const playerId = userClaims.email;
    const baseUrl = environment.apiUrl;
    this.gameConnection = new signalR.HubConnectionBuilder().withUrl(`${baseUrl}/gameHub`).build();

    this.gameConnection.on("Hand", (data: Card[]) => {
      this.player.hand = data;
      this.sortHand();
    });

    this.gameConnection.on("UpdateDiscardPile", (card: Card) => {
      this.discardPile = card;
    });

    this.gameConnection.on("UpdateActivePlayer", (playerId: string) => {
      this.activePlayer = playerId;
      if (this.player.id === this.activePlayer) {
        this.player.stage = Stage.Draw;
        this.snackBar.open("It's your turn!", null, {
          duration: 2000
        });
      }
    });

    this.gameConnection.on("UpdateCompetitors", (updatedCompetitor: PlayerResourceModel) => {
      let index = this.competitors.findIndex(competitor => competitor.id === updatedCompetitor.id);
      this.competitors[index] = updatedCompetitor;
    });

    this.gameConnection.on("UpdateSelf", () => {
      this.gameConnection.invoke<PlayerResourceModel>("GetPlayer", this.gameId, playerId)
        .then((player: PlayerResourceModel) => {
          this.player = player;
          this.sortHand();
        });
    });

    this.gameConnection.on("RequestCompetitors", () => {
      this.gameConnection.invoke<PlayerResourceModel[]>("GetCompetitors", this.gameId, playerId)
          .then((competitors: PlayerResourceModel[]) => {
            this.competitors = competitors;
          });
    });

    this.gameConnection.on("NotifyRoundOver", (totals: ScoreCardResourceModel[]) => {
      console.log('totals', totals);
      const scoreDisplay = this.bottomSheet.open(ScoreCardComponent, {
        data: totals
      });

      scoreDisplay.afterDismissed().subscribe(() => {
        this.startRound(this.gameId, this.player.id);
      })
      
    });

    this.gameConnection.on("NotifyGameWon", (totals: ScoreCardResourceModel[]) => {
      console.log('totals', totals);
      throw new Error('Not yet implemented');
    });

    this.gameConnection.start().then(() => {
      this.gameConnection.send("JoinGame", this.gameId)
        .then(() => {
          this.startRound(this.gameId, playerId);
        });
    });
  }

  private startRound(gameId: string, playerId: string): void {
    this.gameConnection.send("RequestDiscardPile", gameId);
    this.gameConnection.send("RequestActivePlayer", gameId);

    this.gameConnection.invoke<PlayerResourceModel>("GetPlayer",gameId, playerId)
    .then((player: PlayerResourceModel) => {
      this.player = player;
      this.sortHand();
    });

    this.gameConnection.invoke<PlayerResourceModel[]>("GetCompetitors", gameId, playerId)
    .then((competitors: PlayerResourceModel[]) => {
      this.competitors = competitors;
    });
  }
}
