<template>
  <v-container fluid>
    <v-row class="ma-0" style="flex-wrap: wrap; gap: 16px;">
      <v-col v-for="n in 6"
             :key="n"
             cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto"
                width="250"
                :to="`/category/${n}`">
          <v-img class="align-end text-white"
                 height="150"
                 src="https://cdn.vuetifyjs.com/images/cards/docks.jpg"
                 color="secondary"
                 cover>
            <v-card-title>Category</v-card-title>
          </v-img>
        </v-card>
      </v-col>

      <v-col cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto"
                width="250"
                @click="dialog=true">
          <v-img class="align-end text-white"
                 height="150"
                 color="secondary"
                 cover>
            <v-card-title>Create new</v-card-title>
          </v-img>
        </v-card>
      </v-col>

      <v-dialog v-model="dialog" max-width="500">
        <v-card>
          <v-card-title>New item</v-card-title>
          <v-card-text>
            <v-text-field label="Name" />
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn text @click="dialog = false">Cancel</v-btn>
            <v-btn color="primary" @click="save">Create New Category</v-btn>
            <v-btn color="primary" @click="save">Create New SOP</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </v-container>

  <v-snackbar v-model="snackbar" :timeout="3000" color="success">
    Category created successfully!
    <template #actions>
      <v-btn color="white" variant="text" @click="snackbar = false">
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>


<script setup>
import { ref } from 'vue'

const dialog = ref(false)
const snackbar = ref(false)

function save() {
  // handle save logic
  dialog.value = false
  snackbar.value = true
}
</script>

<style>
</style>
