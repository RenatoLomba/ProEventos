import { SocialNetwork } from "./SocialNetwork";
import { SpeakerEvent } from "./SpeakerEvent";

export interface Speaker {
  id: number;
  name: string;
  curriculum: string;
  imageUri: string;
  phone: string;
  email: string;
  socialNetworks: SocialNetwork[];
  speakersEvents: SpeakerEvent[];
}
