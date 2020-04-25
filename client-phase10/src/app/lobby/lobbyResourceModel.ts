import { PlayerResourceModel } from '../player/playerResourceModel';

export interface LobbyResourceModel{
    id: string;
    name: string;
    size: number;
    creatorId: string;
    players: PlayerResourceModel[];
}