<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <main class="home">
    <div class="hero">
      <h1>Welcome to Event Management System</h1>
      <p>Discover and register for exciting events in your area</p>
      <RouterLink to="/events" class="cta-button">Browse Events</RouterLink>
    </div>
    
    <div class="features">
      <div class="feature">
        <h3>Find Events</h3>
        <p>Browse through a variety of events and filter by date and location</p>
      </div>
      
      <div class="feature">
        <h3>Easy Registration</h3>
        <p>Simple registration process with just your basic information</p>
      </div>
      
      <div class="feature">
        <h3>Stay Updated</h3>
        <p>Opt-in to receive updates about future events that interest you</p>
      </div>
    </div>
  </main>
</template>

<style scoped>
.home {
  padding: 40px 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.hero {
  text-align: center;
  padding: 60px 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 12px;
  margin-bottom: 60px;
}

.hero h1 {
  font-size: 3rem;
  margin-bottom: 20px;
  font-weight: bold;
}

.hero p {
  font-size: 1.2rem;
  margin-bottom: 30px;
  opacity: 0.9;
}

.cta-button {
  display: inline-block;
  padding: 15px 30px;
  background-color: white;
  color: #667eea;
  text-decoration: none;
  border-radius: 6px;
  font-weight: bold;
  font-size: 1.1rem;
  transition: transform 0.2s;
}

.cta-button:hover {
  transform: translateY(-2px);
}

.features {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 40px;
  margin-top: 40px;
}

.feature {
  text-align: center;
  padding: 30px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.feature h3 {
  color: #333;
  margin-bottom: 15px;
  font-size: 1.5rem;
}

.feature p {
  color: #666;
  line-height: 1.6;
}

@media (max-width: 768px) {
  .hero h1 {
    font-size: 2rem;
  }
  
  .hero p {
    font-size: 1rem;
  }
  
  .features {
    grid-template-columns: 1fr;
    gap: 20px;
  }
}
</style>
