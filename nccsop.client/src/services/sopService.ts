import axios from 'axios';
import type { AxiosResponse } from 'axios';
import type { SOPItem, SOP } from '@/interfaces';

// Base API URL
export const API_URL = import.meta.env.VITE_API_URL + '/sop';
export function insertSOPItem(item: SOPItem) {
    // Apparently if sending image, can only use FormData
    // But if no image, then just json
    // which i guess makes sense lol
    const formData = new FormData();
    formData.append("Name", item.name);
    formData.append("SopId", item.sopId.toString());
    formData.append("Content", item.content);
    formData.append("SortOrder", item.sortOrder.toString());
    formData.append("Image", item.image ? item.image : "");

    return axios.post(`${API_URL}/${item.sopId}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
}

export function getSOPDetails(id: string | number): Promise<AxiosResponse<SOP>> {
  return axios.get(`${API_URL}/${id}`);
}

export function getAllSOPs() {
  return axios.get(API_URL);
}

export function getImage(imageName: string) {
  return axios.get(`${API_URL}/image/${imageName}`)
}

export function createSOP(sop: SOP) {
  return axios.post(API_URL, sop);
}

export function deleteSOPDetail(sopId: number, detailId: number) {
  return axios.delete(`${API_URL}/${sopId}/${detailId}`);
}

export function deleteSOP(id: number) {
  return axios.delete(`${API_URL}/${id}`);
}

export async function getFileFromServer(filename: string) {
  const response = await axios.get(filename, {
    responseType: 'blob' 
  });
  return response.data; 
}

export function updateSOP(id: number, updatedSop: SOP) {
  console.log(`${API_URL}/${id}`);
  console.log(`${updatedSop.id}`);
  return axios.put(`${API_URL}/${id}`, updatedSop);
}

export function updateSOPItem(item: SOPItem) {
  const formData = new FormData();
  formData.append("Name", item.name);
  formData.append("Id", item.id.toString());
  formData.append("Content", item.content);
  formData.append("SortOrder", item.sortOrder.toString());
  if (item.image) {
    formData.append("Image", item.image);
  }
  formData.append("ImagePath", item.imagePath ? item.imagePath : "");

  return axios.put(`${API_URL}/item/${item.id}`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}


