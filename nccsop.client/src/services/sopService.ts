import axios from 'axios';

// Base API URL
export const API_URL = import.meta.env.VITE_API_URL + '/sop';

interface SOP {
  name: string;
  categoryId: number;   
}

interface SOPItem {
  name: string;
  sopId: number;
  content: string;
  sortOrder: number;
  image?: File; // make it optional if sometimes no file
}

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

export function getSOPDetails(id: number) {
  if (id == null) return [];
  return axios.get(`${API_URL}/${id}`)
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


