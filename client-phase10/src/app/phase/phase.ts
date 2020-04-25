import { CardCombination } from './cardCombination';

export interface Phase {
    playerId: string;
    phaseType: PhaseType;
    cardCombinations: CardCombination[];
}

export enum PhaseType {
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten
} 