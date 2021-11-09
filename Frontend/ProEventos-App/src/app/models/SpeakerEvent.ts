import { Speaker } from "./Speaker";

export interface SpeakerEvent {
  speakerId: number;
  speaker: Speaker;
  eventId: number;
  event: Event;
}
