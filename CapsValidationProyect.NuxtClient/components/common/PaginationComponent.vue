<template>
  <div class="d-inline-flex">
    <ul class="pagination">
      <!-- Botón para mostrar menos páginas -->
      <li v-if="currentPage > maxVisiblePages" class="page-item" @click.prevent="showLessPages()">
        <a class="page-link" href="#"> {{ "<<" }} </a>
      </li>

      <!-- Botón de página anterior -->
      <li :class="{ disabled: currentPage === 1 }" class="page-item"
        @click.prevent="currentPage !== 1 && setCurrentPage(currentPage - 1)">
        <a class="page-link" href="#"> {{ "<" }} </a>
      </li>

      <!-- Páginas visibles -->
      <li v-for="pageNumber in displayedPages" :key="pageNumber" class="page-item"
        :class="{ active: currentPage === pageNumber }">
        <a class="page-link" href="#" @click.prevent="setCurrentPage(pageNumber)">{{
          pageNumber
        }}</a>
      </li>

      <!-- Botón de página siguiente -->
      <li :class="{ disabled: currentPage === totalPages }" class="page-item"
        @click.prevent="currentPage !== totalPages && setCurrentPage(currentPage + 1)">
        <a class="page-link" href="#">></a>
      </li>

      <!-- Botón para mostrar más páginas -->
      <li v-if="displayedPages[displayedPages.length - 1] < totalPages" class="page-item"
        @click.prevent="showNextPages()">
        <a class="page-link" href="#">>></a>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, computed } from "vue";

const emit = defineEmits(["update:currentPage", "update:totalPages"]);

const props = defineProps({
  currentPage: {
    type: Number,
    default: 1
  },
  totalItems: {
    type: Number,
    default: 1
  },
  itemsPerPage: {
    type: Number,
    default: 10
  },
  setCurrentPage: {
    type: Function,
    default: () => { }
  },
  maxVisiblePages: {
    type: Number,
    default: 10
  }
});

const totalPages = computed(() => {
  return Math.ceil(props.totalItems / props.itemsPerPage);
});

const pages = computed(() => {
  const total = totalPages.value;
  const current = props.currentPage;
  const maxVisible = props.maxVisiblePages;
  const start = Math.max(1, current - Math.floor(maxVisible / 2));
  const end = Math.min(total, start + maxVisible - 1);

  return Array.from({ length: end - start + 1 }, (_, index) => start + index);
});

const displayedPages = computed(() => {
  const total = totalPages.value;
  const current = props.currentPage;
  const maxVisible = props.maxVisiblePages;
  const pagesArray = pages.value;

  if (pagesArray.length <= maxVisible) {
    return pagesArray;
  }

  const lastPage = total;
  const halfMaxVisible = Math.floor(maxVisible / 2);

  if (current <= halfMaxVisible) {
    return pagesArray.slice(0, maxVisible);
  }

  if (current >= lastPage - halfMaxVisible) {
    return pagesArray.slice(-maxVisible);
  }

  return pagesArray.slice(current - halfMaxVisible - 1, current + halfMaxVisible);
});

const setCurrentPage = (pageNumber: number) => {
  emit("update:currentPage", pageNumber);
};

const showNextPages = () => {
  const lastDisplayedPage = displayedPages.value[displayedPages.value.length - 1];
  const nextPage = lastDisplayedPage + 1;
  setCurrentPage(nextPage);
};

const showLessPages = () => {
  const firstDisplayedPage = displayedPages.value[0];
  setCurrentPage(Math.max(1, firstDisplayedPage - props.maxVisiblePages));
};
</script>
