<template>

  <v-container>
    <v-card class="mb-4">
      <v-card-text>
        <h1>Title of SOP</h1>
      </v-card-text>
      <v-btn text="Button"></v-btn>
    </v-card>
    <v-row>
      <!-- Left column -->
      <v-col cols="6">
        <v-card v-for="n in 5"
                :key="n"
                class="mb-4">
          <v-card-text>Step {{ n }}: Do something</v-card-text>
        </v-card>
        <v-card>

        </v-card>
      </v-col>

      <!-- Right column -->
      <v-col cols="6">
        <v-card v-for="n in 2"
                :key="n"
                class="mb-4">
          <v-card-text>Another column!!! {{ n }}</v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        components: {
        },
        directives: {
        },
        filters: {
        },
        props: {
        },
        data() {
            return {
            }
        },
        computed: {
        },
        watch: {
        },
        beforeCreate() {
        },
        created() {
        },
        beforeMount() {
        },
        mounted() {
        },
        updated() {
        },
        activated() {
        },
        deactivated() {
        },
        beforeDestroy() {
        },
        destroyed() {
        },
        methods: {
        },
    });
</script>

<style>
</style>
