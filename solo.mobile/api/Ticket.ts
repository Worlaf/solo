export interface Ticket {
    id: number;
    closed: boolean;
    customerId: number;
    type: TicketType;
    parkObjectId: number;
    queueNumber: number;
}

export enum TicketType {
    Adult = 0,
    Child = 1,
    Privileged = 2,
}
