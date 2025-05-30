export interface Event {
  id: number
  name: string
  location: string
  date: string
  startTime: string
}

export interface EventRegistration {
  id?: number
  eventId: number
  name: string
  email: string
  pronouns: string
  optInForCommunication: boolean
  registrationDate?: string
}

export interface EventFilter {
  date?: string
  location?: string
}