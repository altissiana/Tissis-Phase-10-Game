import { Card } from '../card/card';
import { CardCombinationType } from './CardCombinationType';

export interface CardCombination {
    length: number;
    cards: Card[];
    cardCombinationType: CardCombinationType;
}