// categoryStore.ts
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL

const fetchCategories = async () => {
  try {
    const response = await axios.get(API_URL + '/category')
    categoriesMap.value = response.data.reduce((map: any, c: any) => {
      map[c.id] = c
      return map
    }, {})
  } catch (err: any) {
    if (axios.isAxiosError(err)) {
      console.error('AxiosError fetching categories:', err.message, err.response?.status, err.response?.data)
    } else {
      console.error('Unknown error fetching categories:', err)
    }
  }
}

export const useCategoryStore = defineStore('categoryStore', () => {
  const categoriesMap = ref<{ [id: number]: number[] }>({})
  const fetchCategories = async () => {
    try {
      const response = await axios.get(API_URL + '/category')
      categoriesMap.value = response.data.reduce((map: any, c: any) => {
        map[c.id] = c
        return map
      }, {})
    } catch (err: any) {
      if (axios.isAxiosError(err)) {
        console.error('AxiosError fetching categories:', err.message, err.response?.status, err.response?.data)
      } else {
        console.error('Unknown error fetching categories:', err)
      }
    }
  }

  const sopsMap = ref<{ [id: number]: number[] }>({})
  const fetchSops = async () => {
    try {
      const response = await axios.get(API_URL + '/sop')
      sopsMap.value = response.data.reduce((map: any, s: any) => {
        map[s.id] = s
        return map
      }, {})
    } catch (err: any) {
      if (axios.isAxiosError(err)) {
        console.error('AxiosError fetching sops:', err.message, err.response?.status, err.response?.data)
      } else {
        console.error('Unknown error fetching sops:', err)
      }
    }
  }

  const fetchSopDetails = async (id: number) => {
    try {
      const response = await axios.get(API_URL + `/sop/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to fetch SOP details:', error);
      return null; // or handle default/fallback value
    }
  };

  const activeCategoryId = ref<number | null>(null)

  // children of current route category
  const childCategories = computed(() => {
    if (!activeCategoryId.value) return []
    return Object.values(categoriesMap.value).filter(
      (c: any) => c.parentCategoryId === activeCategoryId.value
    )
  })

  const childSops = computed(() => {
    if (!activeCategoryId.value) return []
    return Object.values(sopsMap.value).filter(
      (s: any) => s.categoryId === activeCategoryId.value
    )
  })

  const rootCategories = computed(() => {
    return Object.values(categoriesMap.value).filter(
      (c: any) => !c.parentCategoryId // top-level
    )
  })

  const rootSOPs = computed(() => {
    return Object.values(sopsMap.value).filter(
      (s: any) => !s.categoryId
    )
  })

  return {
    categoriesMap,
    sopsMap,
    fetchCategories,
    activeCategoryId,
    childCategories,
    rootCategories,
    fetchSops,
    fetchSopDetails,
    childSops,
    rootSOPs
  }
})
