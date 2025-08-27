<template>
  <v-container style="max-width: 100%; display: flex; justify-content: center;">
    <v-card class="ma-2 pa-4" style="width: 50%;">
      <v-text-field label="Search" clearable variant="solo-filled" style="width:100%" v-model="categoryStore.search"></v-text-field>
      <v-checkbox label="Include Categories" v-model="checked"></v-checkbox>
    </v-card>
  </v-container>
  <v-container style="max-width: 100%; display: flex; justify-content: center;">

    <v-row class="ma-0" style="flex-wrap: wrap; gap: 16px;">

      <!-- Categories -->
      <v-col v-for="cat in categoryStore.searchedCategories"
             :key="cat.id"
             cols="auto"
             v-if="checked"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" :to="`/category/${cat.id}`">
          <v-img class="align-end text-white"
                 height="250"
                 color="grey"
                 cover>
            <div class="position-absolute" style="top: 8px; right: 8px;">
              <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                  <v-btn v-bind="props"
                         icon
                         variant="text"
                         size="small">
                    <v-icon icon="mdi-dots-vertical"></v-icon>
                  </v-btn>
                </template>

                <v-list>
                  <v-list-item @click="dialog_delete = true; selectedType = 'cat'; selectedId = category.id">
                    <v-list-item-title style="color: red;">Delete</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>

            <v-card-text class="text-h6">
              {{ cat.name }}
            </v-card-text>

          </v-img>
        </v-card>
      </v-col>

      <!-- Searched SOPs -->
      <v-col v-for="sops in categoryStore.searchedSOPs"
             :key="sops.id"
             cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" :to="`/sop/${sops.id}`">
          <v-img class="align-end text-white"
                 height="250"
                 color="cyan"
                 cover>
            <div class="position-absolute" style="top: 8px; right: 8px;">
              <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                  <v-btn v-bind="props"
                         icon
                         variant="text"
                         size="small">
                    <v-icon icon="mdi-dots-vertical"></v-icon>
                  </v-btn>
                </template>

                <v-list>
                  <v-list-item @click="dialog_delete = true; selectedType = 'sop'; selectedId = sops.id">
                    <v-list-item-title style="color: red;">Delete</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>

            <v-card-text class="text-h6">
              {{ sops.name }}
            </v-card-text>

          </v-img>
        </v-card>
      </v-col>
    </v-row>
  </v-container>

  <!-- Delete Dialog-->
  <v-dialog v-model="dialog_delete" max-width="500">
    <v-card>
      <v-card-title class="ma-2">
        Delete
      </v-card-title>
      <v-card-text>
        Are you sure?  This is an irreversible action.
        <v-card-text class="text-red" v-if="selectedType=='cat'">
          All SOPs associated with this category will also be deleted.  You cannot delete categories that contain subcategories.
        </v-card-text>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn text @click="dialog_delete = false">Cancel</v-btn>
        <v-btn color="red" @click="deleteItem()">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>

  <!-- Snackbar -->
  <v-snackbar v-model="snackbar" :timeout="3000" color="danger">
    {{ snackbarMessage }}
    <template #actions>
      <v-btn color="white" variant="text" @click="snackbar = false">
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import { useCategoryStore } from '@/stores/categoryStore'
  import { deleteSOP } from '@/services/sopService'
  import { updateCategory } from '@/services/categoryService'
  import type { Category } from '@/interfaces'


  const categoryStore = useCategoryStore()
  const checked = ref(false)
  const dialog_delete = ref(false)
  const selectedType = ref('')
  const selectedId = ref(0)
  const snackbar = ref(false)
  const snackbarMessage = ref('')


  async function deleteItem() {
    if (selectedType.value == "cat") {
      try {
        const response = await deleteCategory(selectedId.value)
        snackbarMessage.value = 'Category successfully deleted!'
        delete categoryStore.categoriesMap[selectedId.value]
      }
      catch (error: unknown) {
        if (error instanceof Error) {
          snackbarMessage.value = error.message
        } else {
          snackbarMessage.value = String(error)
        }
      }
    }
    else if (selectedType.value == "sop") {
      try {
        const response = await deleteSOP(selectedId.value)
        snackbarMessage.value = 'SOP successfully deleted!'
        delete categoryStore.sopsMap[selectedId.value]
      }
      catch (error: unknown) {
        if (error instanceof Error) {
          snackbarMessage.value = error.message
        } else {
          snackbarMessage.value = String(error)
        }
      }
    }
    dialog_delete.value = false
    snackbar.value = true
  }



</script>

<style>
</style>
