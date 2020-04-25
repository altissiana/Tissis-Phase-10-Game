import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { CardCombination } from '../phase/cardCombination';

@Component({
  selector: 'app-card-combination',
  templateUrl: './card-combination.component.html',
  styleUrls: ['./card-combination.component.css']
})
export class CardCombinationComponent implements OnInit {

  @Input() cardCombination: CardCombination;
  @Input() isCompetitorCombo: boolean;
  cardCombinationType: string;

  constructor() {
    this.cardCombination = {} as CardCombination;
   }

  ngOnInit(): void {
  }

  ngOnChanges(simpleChanges: SimpleChanges) {
    this.cardCombination = simpleChanges.cardCombination.currentValue;

    if(this.cardCombination) {
      this.cardCombinationType = this.getCardCombinationType();
    }
  }

  

  getCardCombinationType() {
    console.log('card combination', this.cardCombination);
    let comboType: string;
    switch (this.cardCombination.cardCombinationType) {
      case 0: {
        comboType = 'Set';
        break;
      }
      case 1: {
        comboType = 'Run';
        break;
      }
      case 2: {
        comboType = 'Color';
        break;
      }
      default: {
        comboType = 'Invalid'
        break;
      }
    }

    return comboType;
  }
}
