import axios from 'axios';
import type { Category } from '@/interfaces'; 

// Base API URL (adjust if your backend runs on a different port)
const API_URL = import.meta.env.VITE_API_URL + '/category';

export function getAllCategories() {
  return axios.get(API_URL);
}

export function getCategoryById(id: number) {
  return axios.get(`${API_URL}/${id}`);
}

export function createCategory(category: Category) {
  return axios.post(API_URL, category);
}

export function updateCategory(id: number, category: Category) {
  console.log(`${API_URL}/${id}`);
  return axios.put(`${API_URL}/${id}`, category);
}

export function deleteCategory(id: number) {
  return axios.delete(`${API_URL}/${id}`);
}
