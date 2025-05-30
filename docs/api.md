# API Documentation

## Overview

The Event Management System provides a RESTful API for managing events and registrations.

## Base URL

```
Local Development: http://localhost:7071/api
Production: https://your-function-app.azurewebsites.net/api
```

## Authentication

Currently, the API uses anonymous authentication for development purposes. In production, you should implement proper authentication mechanisms.

## Events API

### Get All Events

```http
GET /events
```

**Query Parameters:**
- `date` (optional): Filter events by date (YYYY-MM-DD format)
- `location` (optional): Filter events by location (partial match)

**Response:**
```json
[
  {
    "id": 1,
    "name": "Tech Conference 2024",
    "location": "San Francisco, CA",
    "date": "2024-06-15T00:00:00Z",
    "startTime": "09:00:00"
  }
]
```

### Get Event by ID

```http
GET /events/{id}
```

**Path Parameters:**
- `id`: Event ID (integer)

**Response:**
```json
{
  "id": 1,
  "name": "Tech Conference 2024",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

### Create Event

```http
POST /events
```

**Request Body:**
```json
{
  "name": "Tech Conference 2024",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

**Response:** 201 Created
```json
{
  "id": 1,
  "name": "Tech Conference 2024",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

### Update Event

```http
PUT /events/{id}
```

**Path Parameters:**
- `id`: Event ID (integer)

**Request Body:**
```json
{
  "name": "Tech Conference 2024 - Updated",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

**Response:** 200 OK
```json
{
  "id": 1,
  "name": "Tech Conference 2024 - Updated",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

### Delete Event

```http
DELETE /events/{id}
```

**Path Parameters:**
- `id`: Event ID (integer)

**Response:** 204 No Content

## Error Responses

The API returns standard HTTP status codes:

- `200` - Success
- `201` - Created
- `204` - No Content
- `400` - Bad Request
- `404` - Not Found
- `500` - Internal Server Error

**Error Response Format:**
```json
{
  "error": "Error message description"
}
```

## Rate Limiting

Currently, no rate limiting is implemented. In production, consider implementing rate limiting to prevent abuse.

## CORS

The API supports CORS for cross-origin requests from the frontend application.