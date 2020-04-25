import { Card } from '../card/card';
import { Phase, PhaseType } from '../phase/phase';
import { Stage } from '../game/stage';

export interface PlayerResourceModel{
    id: string;
    name: string;
    hand: Card[];
    phase: Phase;
    stage: Stage;
    hasCompletedPhase: boolean;
}