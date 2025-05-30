<template>
  <div class="events-list">
    <h1>Events</h1>
    
    <!-- Filter Controls -->
    <div class="filters">
      <div class="filter-group">
        <label for="date-filter">Filter by Date:</label>
        <input
          id="date-filter"
          v-model="filters.date"
          type="date"
          @change="applyFilters"
        />
      </div>
      
      <div class="filter-group">
        <label for="location-filter">Filter by Location:</label>
        <input
          id="location-filter"
          v-model="filters.location"
          type="text"
          placeholder="Enter location"
          @input="applyFilters"
        />
      </div>
      
      <button @click="clearFilters" class="clear-btn">Clear Filters</button>
    </div>

    <!-- Events Grid -->
    <div v-if="loading" class="loading">Loading events...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else-if="events.length === 0" class="no-events">No events found.</div>
    <div v-else class="events-grid">
      <div
        v-for="event in events"
        :key="event.id"
        class="event-card"
        @click="viewEventDetails(event.id)"
      >
        <h3>{{ event.name }}</h3>
        <p><strong>Location:</strong> {{ event.location }}</p>
        <p><strong>Date:</strong> {{ formatDate(event.date) }}</p>
        <p><strong>Start Time:</strong> {{ event.startTime }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { EventService } from '@/services/EventService'
import type { Event, EventFilter } from '@/types/Event'

const router = useRouter()

const events = ref<Event[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const filters = ref<EventFilter>({
  date: '',
  location: ''
})

const loadEvents = async () => {
  loading.value = true
  error.value = null
  
  try {
    const filterParams = {
      ...(filters.value.date && { date: filters.value.date }),
      ...(filters.value.location && { location: filters.value.location })
    }
    
    events.value = await EventService.getEvents(filterParams)
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'An error occurred'
  } finally {
    loading.value = false
  }
}

const applyFilters = () => {
  loadEvents()
}

const clearFilters = () => {
  filters.value = { date: '', location: '' }
  loadEvents()
}

const viewEventDetails = (eventId: number) => {
  router.push(`/events/${eventId}`)
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString()
}

onMounted(() => {
  loadEvents()
})
</script>

<style scoped>
.events-list {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.filters {
  display: flex;
  gap: 20px;
  margin-bottom: 30px;
  padding: 20px;
  background-color: #f5f5f5;
  border-radius: 8px;
  flex-wrap: wrap;
  align-items: end;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.filter-group label {
  font-weight: bold;
  color: #333;
}

.filter-group input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.clear-btn {
  padding: 8px 16px;
  background-color: #6c757d;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.clear-btn:hover {
  background-color: #5a6268;
}

.loading,
.error,
.no-events {
  text-align: center;
  padding: 40px;
  font-size: 18px;
}

.error {
  color: #dc3545;
}

.events-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.event-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
  background-color: white;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.event-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  border-color: #007bff;
}

.event-card h3 {
  margin: 0 0 10px 0;
  color: #007bff;
  font-size: 18px;
}

.event-card p {
  margin: 5px 0;
  color: #666;
  font-size: 14px;
}

.event-card strong {
  color: #333;
}
</style>