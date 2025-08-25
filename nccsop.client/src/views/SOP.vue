<template>

  <v-container>
    <v-card class="mb-2" >
      <v-card-text>
        <h1>{{details?.name}}</h1>
      </v-card-text>
      <v-btn text="NEW ITEM" class="ma-4 mt-0" color="primary" @click="dialog_newitem = true"></v-btn>
      <v-btn text="BACK" class="ma-4 mt-0" color="grey" @click="$router.go(-1)"></v-btn>
    </v-card>
    <v-row  justify="center" class="w-100" style="max-width: 1200px;">
      <!-- Left column -->
      <v-col cols="12" md="6" style="width: 1200px;">
        <v-skeleton-loader v-if="loading" type="image"></v-skeleton-loader>
        <v-card v-for="step in details?.sopItems"
                :key="'left-' + step"
                class="mb-4"
                >
          <!-- Edit/Delete section-->
          <div class="position-absolute" style="top: 8px; right: 8px;">
            <v-menu open-on-hover>
              <template v-slot:activator="{ props }">
                <v-btn v-bind="props" icon variant="text" size="small">
                  <v-icon icon="mdi-dots-vertical"></v-icon>
                </v-btn>
              </template>

              <v-list>
                <v-list-item @click="">
                  <v-list-item-title>Edit</v-list-item-title>
                </v-list-item>
                <v-list-item @click="dialog_delete = true; item = step.id">
                  <v-list-item-title style="color: red;">Delete</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>

          <v-row class="align-center">
            <v-col class="pb-0">
              <v-card-title class="d-flex align-center">
                <div class="d-flex flex-column">
                  <v-icon small icon="mdi-menu-up"></v-icon>
                  <v-icon small icon="mdi-menu-down"></v-icon>
                </div>
                <span class="ma-2 ">{{ step.name }}</span>
              </v-card-title>
            </v-col>
          </v-row>

          <v-row>
            <v-col cols="8" class="pt-0">
              <v-card-text>
                {{step.content}}
              </v-card-text>
            </v-col>
            <v-col cols="4" class="pa-5 d-flex align-center justify-center">
              <v-img :src="step.imageUrl" @click="openDialog(step.imageUrl)" class="cursor-pointer"></v-img>
            </v-col>
          </v-row>
        </v-card>
      </v-col>



      <!-- Right column -->
      <v-col cols="12" md="6">
        <v-skeleton-loader v-if="loading" type="image"></v-skeleton-loader>
        <v-card v-for="step in (details?.sopItems || []).filter(s => s.imageUrl)"
                :key="'right-' + step.id"
                class="mb-4">
          <!-- Edit/Delete section-->
          <div class="position-absolute" style="top: 8px; right: 8px;">
            <v-menu open-on-hover>
              <template v-slot:activator="{ props }">
                <v-btn v-bind="props" icon variant="text" size="small">
                  <v-icon icon="mdi-dots-vertical"></v-icon>
                </v-btn>
              </template>

              <v-list>
                <v-list-item @click="">
                  <v-list-item-title>Edit</v-list-item-title>
                </v-list-item>
                <v-list-item @click="dialog_delete = true; item = step.id">
                  <v-list-item-title style="color: red;">Delete</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>

          <v-img :src="step.imageUrl" @click="openDialog(step.imageUrl)" class="cursor-pointer"></v-img>

        </v-card>

          
      </v-col>
    </v-row>
  </v-container>

  <!-- Image modal shared by all -->
  <v-dialog v-model="dialog" max-width="800">
    <v-card>
      <v-img :src="selectedImage" contain max-height="600" />
    </v-card>
  </v-dialog>

  <v-snackbar-queue v-model="toast"></v-snackbar-queue>

  <!-- SOPItem Creation Modal -->
  <v-dialog v-model="dialog_newitem" max-width="800">
    <v-card>
      <v-card-actions class="pa-4">
        <v-col>
          <v-row>
            <v-text-field label="Name"
                          v-model="name"
                          variant="outlined"></v-text-field>
          </v-row>

          <v-row>
            <v-textarea v-model="content"
                        label="Description"
                        single-line></v-textarea>
          </v-row>

          <v-row>
          </v-row>

          <v-row>
            <v-file-input v-model="file" label="Image upload (optional)" clearable></v-file-input>
          </v-row>

          <v-row>
            <v-btn text="Close" @click="dialog_newitem = false"></v-btn>
            <v-btn text="Create" @click="insertItem()" color="success"></v-btn>
          </v-row>
        </v-col>

      </v-card-actions>
    </v-card>
  </v-dialog>

  <!-- Delete step modal -->
  <v-dialog v-model="dialog_delete" max-width="500">
    <v-card>
      <v-card-title class="ma-2">
        Delete
      </v-card-title>
      <v-card-text>
        Are you sure?  This is an irreversible action.
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn text @click="dialog_delete = false">Cancel</v-btn>
        <v-btn color="red" @click="deleteItem()">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>

</template>


<script setup lang="ts">
  import { onMounted, ref } from "vue";
  import { insertSOPItem, getSOPDetails, deleteSOPDetail, API_URL } from "../services/sopService"
  import { useRoute } from 'vue-router'
  const route = useRoute()

  interface SOPItem {
    id: number;
    sopId: number;
    name: string;
    content: string;
    imagePath?: string;
    stepNumber?: number;
    sortOrder: number;
    imageUrl?: string;
  }

  interface SOP {
    id: number;
    name: string;
    sopItems: SOPItem[];
    categoryId?: number;
    createdAt: string;
    updatedAt: string;
  }

  const details = ref<SOP | null>(null);
  onMounted(async () => {
    const res = await getSOPDetails(route.params.sopId);
    fetchSopDetails()
  })

  async function fetchSopDetails() {
    try {
      const res = await getSOPDetails(route.params.sopId);
      details.value = res.data
      details.value.sopItems.sort((a, b) => a.sortOrder - b.sortOrder);
      details.value.sopItems.forEach(item => {
        if (item.imagePath != null && item.imagePath !== '') {
          item.imageUrl = `${API_URL}/image/${item.imagePath}`;
        }
      });
    } catch (error) {
      console.log(error)
    }
  }

  const loading = ref(false);
  const dialog = ref(false);
  const dialog_newitem = ref(false);
  const dialog_delete = ref(false);
  const item = ref(0);
  const selectedImage = ref("");

  const name = ref("");
  const content = ref("");
  const file = ref<File | null>(null);
  const toast = ref("");

  function openDialog(src: string) {
    selectedImage.value = src;
    dialog.value = true;
  }

  async function deleteItem() {
    await deleteSOPDetail(route.params.sopId, item.value);
    fetchSopDetails()
    dialog_delete.value = false;
  }

  async function insertItem() {
    console.log(name.value + " " + content.value)

    const lastItem = details.value.sopItems?.[details.value.sopItems.length - 1];
    const sortOrder = lastItem ? lastItem.sortOrder + 1 : 1;
    try {
      const response = await insertSOPItem({
        sopId: route.params.sopId,
        name: name.value,
        content: content.value,
        sortOrder: sortOrder,
        image: file.value ? file.value : null
      });

      fetchSopDetails()

      console.log(response.data)
    } catch (error) {
      console.log(error)
    }
    dialog_newitem.value = false
    name.value = "";
    content.value = "";
    file.value = null;
  }

</script>

<style>
</style>
