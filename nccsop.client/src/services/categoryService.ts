import axios from 'axios';

// Base API URL (adjust if your backend runs on a different port)
const API_URL = 'https://localhost:7269/api/category';

export function getAllCategories() {
  return axios.get(API_URL);
}

export function getCategoryById(id) {
  return axios.get(`${API_URL}/${id}`);
}

export function createCategory(category) {
  return axios.post(API_URL, category);
}

export function updateCategory(id, category) {
  return axios.put(`${API_URL}/${id}`, category);
}

export function deleteCategory(id) {
  return axios.delete(`${API_URL}/${id}`);
}
