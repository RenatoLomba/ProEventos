import { Batch } from "./Batch";
import { SocialNetwork } from "./SocialNetwork";
import { SpeakerEvent } from "./SpeakerEvent";

export interface Event {
  id: number;
  place: string;
  eventDate?: Date;
  theme: string;
  peopleQty: number;
  imageUri: string;
  phone: string;
  email: string;
  batches: Batch[];
  socialNetworks: SocialNetwork[];
  speakersEvents: SpeakerEvent[];
}
