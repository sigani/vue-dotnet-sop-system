// categoryStore.ts
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import axios from 'axios'

export const useCategoryStore = defineStore('categoryStore', () => {
  const categoriesMap = ref<{ [id: number]: any }>({})
  const fetchCategories = async () => {
    const response = await axios.get('https://localhost:7269/api/category')
    categoriesMap.value = response.data.reduce((map: any, c: any) => {
      map[c.id] = c
      return map
    }, {})
  }

  const sopsMap = ref<{ [id: number]: any }>({})
  const fetchSops = async () => {
    const response = await axios.get('https://localhost:7269/api/sop')
    sopsMap.value = response.data.reduce((map: any, s: any) => {
      map[s.id] = s
      return map
    }, {})
  }

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
    childSops,
    rootSOPs
  }
})
