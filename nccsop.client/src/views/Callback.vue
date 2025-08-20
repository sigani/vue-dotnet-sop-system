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
  <div>Logging in...</div>
</template>
