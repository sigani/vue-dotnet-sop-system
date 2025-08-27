<template>

  <v-container style="width:1920px">
    <v-card class="mb-2">
      <v-card-text>
        <h1>{{details?.name}}</h1>
      </v-card-text>
      <v-btn text="NEW ITEM" class="ma-4 mt-0" color="primary" @click="dialog_newitem = true" v-if="edit_mode"></v-btn>
      <v-btn text="EDIT" class="ma-4 mt-0" color="grey" @click="edit_mode=!edit_mode" v-if="!edit_mode"></v-btn>
      <v-btn text="SAVE CHANGES" class="ma-4 mt-0" color="green" @click="saveChanges()" v-if="edit_mode"></v-btn>
      <v-btn text="CANCEL" class="ma-4 mt-0" color="red" @click="edit_mode=!edit_mode" v-if="edit_mode"></v-btn>
      <v-btn text="BACK" class="ma-4 mt-0" variant="outlined" @click="$router.go(-1)"></v-btn>
    </v-card>
    <v-row  justify="center" class="w-auto" style="max-width: 1920px;">
      <!-- Left column -->
      <v-col cols="12" md="6" style="width: 1800px;">
        <v-skeleton-loader v-if="loading" type="image"></v-skeleton-loader>
        <v-card v-for="(step, index) in validSteps"
                :key="'left-' + step"
                class="mb-1"
                >
          <!-- Edit/Delete section-->
          <div class="position-absolute" style="top: 8px; right: 8px;" v-if="edit_mode">
            <v-menu open-on-hover>
              <template v-slot:activator="{ props }">
                <v-btn v-bind="props" icon variant="text" size="small">
                  <v-icon icon="mdi-dots-vertical"></v-icon>
                </v-btn>
              </template>

              <v-list >
                <v-list-item @click="openEditDialog(step)">
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
                <div class="d-flex flex-column ma-0" v-if="edit_mode">
                  <v-btn icon="mdi-menu-up" variant="text" size="medium" density="compact" v-if="index != 0" @click="changeOrder(index, index-1)"></v-btn>
                  <v-btn icon="mdi-menu-down" variant="text" size="medium" density="compact" v-if="index != validSteps.length-1" @click="changeOrder(index, index+1)"></v-btn>
                </div>
                <span class="ma-2 ">{{ step.name }}</span>
              </v-card-title>
            </v-col>
          </v-row>

          <v-row>
            <v-col :cols="step.imageUrl ? 8 : 12" class="pt-0">
              <v-card-text>
                {{step.content}}
              </v-card-text>
            </v-col>
            <v-col cols="4" class="pa-5 d-flex align-center justify-center" v-if="step.imageUrl">
              <v-img :src="step.imageUrl" @click="step.imageUrl && openDialog(step.imageUrl)" class="cursor-pointer"></v-img>
            </v-col>
          </v-row>
        </v-card>
      </v-col>

      <!-- Right column -->
      <v-col cols="12" md="6">
        <v-skeleton-loader v-if="loading" type="image"></v-skeleton-loader>
        <v-card v-for="step in (details?.sopItems || []).filter(s => s.imageUrl)"
                :key="'right-' + step.id"
                class="mb-4"
                :title="step.name"
                >
          
          <v-img :src="step.imageUrl" @click="step.imageUrl && openDialog(step.imageUrl)" class="cursor-pointer"></v-img>

        </v-card>

          
      </v-col>
    </v-row>
  </v-container>

  <!-- Image modal shared by all -->
  <v-dialog v-model="dialog" max-width="1600">
    <v-card style="position: relative; ">
      <v-img :src="selectedImage"
             max-height="800"
             max-width="100%"
             contain/>
      <v-btn icon="$close"
             size="large"
             variant="text"
             @click="dialog=!dialog"
             style="position: absolute; top: 8px; right: 8px;"
             ></v-btn>
    </v-card>
  </v-dialog>


  <!-- SOPItem Creation/Edit Modal -->
  <v-dialog v-model="dialog_newitem" max-width="800" persistent>
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
            <v-btn text="Create" @click="insertItem()" color="success" v-if="!edit_item_mode"></v-btn>
            <v-btn text="Update" @click="editItem()" color="primary" v-if="edit_item_mode"></v-btn>
            <v-btn text="Close" @click="dialog_newitem = false; edit_item_mode = false"></v-btn>
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
  import { onMounted, ref, computed } from "vue";
  import { insertSOPItem, getSOPDetails, deleteSOPDetail, updateSOPItem, API_URL, getFileFromServer } from "../services/sopService"
  import { useRoute } from 'vue-router'
  import type { SOPItem, SOP } from '@/interfaces'
  import type { AxiosResponse } from "axios";
  const route = useRoute()
  const details = ref<SOP | null>(null);
  const loading = ref(false);
  const dialog = ref(false);
  const dialog_newitem = ref(false);
  const dialog_delete = ref(false);
  const item = ref(0);
  const selectedImage = ref("");

  const edit_mode = ref(false)
  const edit_item_mode = ref(false)
  const selected_item = ref<SOPItem | null>(null)

  const name = ref("");
  const content = ref("");
  const file = ref<File | null>(null);

  onMounted(async () => {
    await fetchSopDetails()
  })

  const validSteps = computed(() => {
    if (!details.value?.sopItems) return [];
    if (edit_mode.value) return details.value.sopItems;
    return details.value.sopItems.filter(
      step => (step.name?.trim() || step.content?.trim())
    );
  });

  // literally a swap
  function changeOrder(first: number, second: number) {
    if (details.value && details.value.sopItems) {
      const temp: SOPItem = details.value.sopItems[first];
      details.value.sopItems[first] = details.value.sopItems[second];
      details.value.sopItems[second] = temp;
    }
  }

  function saveChanges() {
    try {
      if (details.value && details.value.sopItems) {
        details.value.sopItems.forEach(async (item, index) => {
          item.sortOrder = index;
          await updateSOPItem(item);
        })
      }
    } catch (error) {
      console.log(error)
    }
    edit_mode.value = false;
  }

  async function fetchSopDetails() {
    try {
      const res: AxiosResponse<SOP> = await getSOPDetails(Number(route.params.sopId));
      details.value = res.data
      if (details.value) {
        details.value.sopItems.sort((a, b) => a.sortOrder - b.sortOrder);
        details.value.sopItems.forEach(item => {
          if (item.imagePath != null && item.imagePath !== '') {
            item.imageUrl = `${API_URL}/image/${item.imagePath}`;
          }
        });
      }

    } catch (error) {
      console.log(error)
    }
  }

  function openDialog(src: string) {
    selectedImage.value = src;
    dialog.value = true;
  }

  async function openEditDialog(item: SOPItem) {
    dialog_newitem.value = true;
    edit_item_mode.value = true;
    selected_item.value = item;

    name.value = item.name;
    content.value = item.content;

    if (item.imageUrl) {
      const blob = await getFileFromServer(item.imageUrl); // get blob
      const filename = item.imageUrl.split('/').pop() || 'file.jpg'; // extract name from URL
      file.value = new File([blob], filename, { type: blob.type, lastModified: Date.now() });
    } else {
      file.value = null;
    }
  }

  async function deleteItem() {
    await deleteSOPDetail(Number(route.params.sopId), item.value);
    fetchSopDetails()
    dialog_delete.value = false;
  }

  async function insertItem() {
    console.log(name.value + " " + content.value)

    const lastItem = details.value?.sopItems?.[details.value.sopItems.length - 1];
    const sortOrder = lastItem ? lastItem.sortOrder + 1 : 1;
    try {
      const response = await insertSOPItem({
        sopId: Number(route.params.sopId),
        name: name.value,
        content: content.value,
        sortOrder: sortOrder,
        image: file.value ? file.value : null
      } as SOPItem);

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

  async function editItem() {
    try {
      var newSop = selected_item.value;
      if (!newSop) return;
      newSop.name = name.value
      newSop.content = content.value
      newSop.image = file.value ? file.value : null
      const response = await updateSOPItem(newSop);

      fetchSopDetails()

      console.log(response.data)
    } catch (error) {
      console.log(error)
    }
    dialog_newitem.value = false
    name.value = "";
    content.value = "";
    file.value = null;
    edit_item_mode.value = false;
    selected_item.value = null;
  }



</script>

<style>
</style>
