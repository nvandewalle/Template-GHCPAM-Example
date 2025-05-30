<template>
  <div class="event-details">
    <div v-if="loading" class="loading">Loading event...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else-if="event" class="event-content">
      <button @click="goBack" class="back-btn">← Back to Events</button>
      
      <div class="event-info">
        <h1>{{ event.name }}</h1>
        <div class="event-meta">
          <p><strong>Location:</strong> {{ event.location }}</p>
          <p><strong>Date:</strong> {{ formatDate(event.date) }}</p>
          <p><strong>Start Time:</strong> {{ event.startTime }}</p>
        </div>
      </div>

      <div class="registration-section">
        <h2>Register for This Event</h2>
        
        <form @submit.prevent="submitRegistration" class="registration-form">
          <div class="form-group">
            <label for="name">Name *</label>
            <input
              id="name"
              v-model="registration.name"
              type="text"
              required
            />
          </div>

          <div class="form-group">
            <label for="email">Email *</label>
            <input
              id="email"
              v-model="registration.email"
              type="email"
              required
            />
          </div>

          <div class="form-group">
            <label for="pronouns">Pronouns</label>
            <input
              id="pronouns"
              v-model="registration.pronouns"
              type="text"
              placeholder="e.g., they/them, she/her, he/him"
            />
          </div>

          <div class="form-group checkbox-group">
            <label>
              <input
                v-model="registration.optInForCommunication"
                type="checkbox"
              />
              I would like to receive further communication about events
            </label>
          </div>

          <button
            type="submit"
            :disabled="registering"
            class="register-btn"
          >
            {{ registering ? 'Registering...' : 'Register' }}
          </button>
        </form>

        <div v-if="registrationSuccess" class="success-message">
          Thank you for registering! We'll see you at the event.
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { EventService } from '@/services/EventService'
import type { Event, EventRegistration } from '@/types/Event'

const route = useRoute()
const router = useRouter()

const event = ref<Event | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)
const registering = ref(false)
const registrationSuccess = ref(false)

const registration = ref<Omit<EventRegistration, 'id' | 'eventId' | 'registrationDate'>>({
  name: '',
  email: '',
  pronouns: '',
  optInForCommunication: false
})

const loadEvent = async () => {
  const eventId = Number(route.params.id)
  if (!eventId) {
    error.value = 'Invalid event ID'
    return
  }

  loading.value = true
  error.value = null

  try {
    event.value = await EventService.getEvent(eventId)
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'An error occurred'
  } finally {
    loading.value = false
  }
}

const submitRegistration = async () => {
  if (!event.value) return

  registering.value = true

  try {
    // In a real application, this would call a registration API
    // For now, we'll just simulate the registration
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    registrationSuccess.value = true
    
    // Reset form
    registration.value = {
      name: '',
      email: '',
      pronouns: '',
      optInForCommunication: false
    }
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Registration failed'
  } finally {
    registering.value = false
  }
}

const goBack = () => {
  router.push('/events')
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('en-US', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

onMounted(() => {
  loadEvent()
})
</script>

<style scoped>
.event-details {
  padding: 20px;
  max-width: 800px;
  margin: 0 auto;
}

.back-btn {
  padding: 8px 16px;
  background-color: #6c757d;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin-bottom: 20px;
  font-size: 14px;
}

.back-btn:hover {
  background-color: #5a6268;
}

.loading,
.error {
  text-align: center;
  padding: 40px;
  font-size: 18px;
}

.error {
  color: #dc3545;
}

.event-content {
  background-color: white;
  border-radius: 8px;
  padding: 30px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.event-info h1 {
  color: #007bff;
  margin-bottom: 20px;
  font-size: 2em;
}

.event-meta p {
  margin: 10px 0;
  font-size: 16px;
  color: #666;
}

.event-meta strong {
  color: #333;
}

.registration-section {
  margin-top: 40px;
  padding-top: 30px;
  border-top: 2px solid #f1f1f1;
}

.registration-section h2 {
  color: #333;
  margin-bottom: 20px;
}

.registration-form {
  max-width: 500px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: #333;
}

.form-group input[type="text"],
.form-group input[type="email"] {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
  box-sizing: border-box;
}

.checkbox-group {
  display: flex;
  align-items: center;
  gap: 8px;
}

.checkbox-group label {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: normal;
  cursor: pointer;
}

.checkbox-group input[type="checkbox"] {
  width: auto;
  margin: 0;
}

.register-btn {
  padding: 12px 24px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  font-weight: bold;
}

.register-btn:hover:not(:disabled) {
  background-color: #0056b3;
}

.register-btn:disabled {
  background-color: #6c757d;
  cursor: not-allowed;
}

.success-message {
  margin-top: 20px;
  padding: 15px;
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
  border-radius: 4px;
  font-weight: bold;
}
</style>