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

  const activeCategoryId = ref<number | null>(null)

  // children of current route category
  const childCategories = computed(() => {
    if (!activeCategoryId.value) return []
    return Object.values(categoriesMap.value).filter(
      (c: any) => c.parentCategoryId === activeCategoryId.value
    )
  })

  const rootCategories = computed(() => {
    return Object.values(categoriesMap.value).filter(
      (c: any) => !c.parentCategoryId // top-level
    )
  })

  return {
    categoriesMap,
    fetchCategories,
    activeCategoryId,
    childCategories,
    rootCategories
  }
})



//import { defineStore } from 'pinia'
//import axios from 'axios'

//export interface Category {
//  id: number
//  name: string
//  parentCategoryId: number | null
//}

//export const useCategoryStore = defineStore('category', {
//  state: () => ({
//    categoriesMap: {} as Record<number, Category>,
//    loaded: false,
//  }),
//  actions: {
//    async fetchCategories() {
//      if (this.loaded) return // avoid fetching twice
//      try {
//        const response = await axios.get<Category[]>('https://localhost:7269/api/category')
//        this.categoriesMap = response.data.reduce((acc, cat) => {
//          acc[cat.id] = cat
//          return acc
//        }, {} as Record<number, Category>)
//        this.loaded = true
//      } catch (error) {
//        console.error('Failed to fetch categories:', error)
//      }
//    }
//  }
//})
