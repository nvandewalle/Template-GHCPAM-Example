import type { Event, EventFilter } from '@/types/Event'

const API_BASE_URL = 'http://localhost:7071/api'

export class EventService {
  static async getEvents(filter?: EventFilter): Promise<Event[]> {
    const params = new URLSearchParams()
    if (filter?.date) params.append('date', filter.date)
    if (filter?.location) params.append('location', filter.location)
    
    const url = `${API_BASE_URL}/events${params.toString() ? '?' + params.toString() : ''}`
    const response = await fetch(url)
    
    if (!response.ok) {
      throw new Error('Failed to fetch events')
    }
    
    return response.json()
  }

  static async getEvent(id: number): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`)
    
    if (!response.ok) {
      throw new Error('Failed to fetch event')
    }
    
    return response.json()
  }

  static async createEvent(event: Omit<Event, 'id'>): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(event),
    })
    
    if (!response.ok) {
      throw new Error('Failed to create event')
    }
    
    return response.json()
  }

  static async updateEvent(id: number, event: Omit<Event, 'id'>): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(event),
    })
    
    if (!response.ok) {
      throw new Error('Failed to update event')
    }
    
    return response.json()
  }

  static async deleteEvent(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`, {
      method: 'DELETE',
    })
    
    if (!response.ok) {
      throw new Error('Failed to delete event')
    }
  }
}