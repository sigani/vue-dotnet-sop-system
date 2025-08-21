import axios from 'axios';

// Base API URL
const API_URL = 'https://localhost:7269/api/sop';

export function getAllSOPs() {
  return axios.get(API_URL);
}

export function createSOP(sop) {
  return axios.post(API_URL, sop);
}
