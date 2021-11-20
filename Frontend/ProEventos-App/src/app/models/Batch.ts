export interface Batch {
  id: number;
  name: string;
  price: number;
  startDate?: Date;
  endDate?: Date;
  qty: number;
  eventId: number;
  event: Event | null;
}
