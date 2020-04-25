import { Component, OnInit, Inject } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA } from '@angular/material/bottom-sheet';
import { ScoreCardResourceModel } from '../player/ScoreCardResourceModel';

@Component({
  selector: 'app-score-card',
  templateUrl: './score-card.component.html',
  styleUrls: ['./score-card.component.css']
})
export class ScoreCardComponent {
  data: ScoreCardResourceModel[];
  winner: string;
  constructor(@Inject(MAT_BOTTOM_SHEET_DATA) data: ScoreCardResourceModel[]) {
    this.data = data;
    this.winner = this.data.find(x => x.roundScore === 0).name;
   }

}
