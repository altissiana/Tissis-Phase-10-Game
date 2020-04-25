import { Component, OnInit, Input } from '@angular/core';
import { Phase } from './phase';
import { CardCombination } from './cardCombination';
import { Card } from '../card/card';
import { PlayerResourceModel } from '../player/playerResourceModel';

@Component({
  selector: 'app-phase',
  templateUrl: './phase.component.html',
  styleUrls: ['./phase.component.css']
})
export class PhaseComponent implements OnInit {

  @Input() phase: Phase;
  @Input() player: PlayerResourceModel;
  @Input() onComboDropCallback: (playerId: string, dropData: Card, cardCombination: CardCombination) => any;

  constructor() {
    this.phase = {} as Phase;
   }

  ngOnInit(): void {
  }

  onComboDropped(dropData: Card, cardCombination: CardCombination) {
    this.onComboDropCallback(this.phase.playerId, dropData, cardCombination);
  }
}
