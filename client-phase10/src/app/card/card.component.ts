import { Component, Input, SimpleChanges } from '@angular/core';
import { Card } from './card';
import { DragDrop } from '@angular/cdk/drag-drop';
import { CardType } from './CardType';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {

  @Input() card: Card;
  @Input() isCompetitorCard: boolean;
  value: string;
  cardStyle: string;

  constructor(private dragDrop: DragDrop) { }

  ngOnChanges(changes: SimpleChanges) {
    this.card = changes.card.currentValue;
    if (this.card) {
      this.setCardType();
      this.setCardColor();
    }
    else {
      this.value = 'EMPTY';
    }
  }

  setCardType() {
    if (this.card?.cardType !== undefined) {
      if(this.card.cardType === 13) {
        this.value = 'Wild';
      }
      else if (this.card.cardType === 14) {
        this.value = 'Skip';
      }
      else if (this.card.cardType === CardType.Deck) {
        this.value = '';
      }
      else {
        this.value = this.card.cardType.toString();
      }
    }
  }

  setCardColor() {
    this.cardStyle = 'single-card ';
    if (this.isCompetitorCard) {
      this.cardStyle += 'single-card-competitor ';
    }

    if (this.card?.cardColor !== undefined) {
      switch (this.card.cardColor) {
        case -1: {
          this.cardStyle += 'deck';
          break;
        }
        case 0: {
          this.cardStyle += "red";
          break;
        }
        case 1: {
          this.cardStyle += "green";
          break;
        }
        case 2: {
          this.cardStyle += "blue";
          break;
        }
        case 3: {
          this.cardStyle += "yellow";
          break;
        }
        default: {
          if (this.card.cardType === 13) {
            this.cardStyle += "wild";
          } else {
            this.cardStyle += "skip";
          }
          break;
        }
      }
    } 
  }
}
