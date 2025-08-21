<script setup lang="ts">
import { onMounted } from 'vue'
import { userManager } from '../authService'

onMounted(async () => {
  try {
    await userManager.signinRedirectCallback()
    window.location.href = '/' // redirect to home after login
  } catch (err) {
    console.error(err)
  }
})
</script>

<template>
  <v-row class="ma-0" style="flex-wrap: wrap; gap: 16px;">
    <v-col v-for="n in 9"
            :key="n"
            cols="auto"
            style="flex: 0 0 calc(20% - 16px);">
      <v-card class="mx-auto" width="270">
        <v-skeleton-loader type="image"
                            height="100%"
                            class="position-relative" />
      </v-card>
    </v-col>
  </v-row>
</template>
