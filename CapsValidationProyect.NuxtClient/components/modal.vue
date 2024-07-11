<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div>
    <!-- Button trigger modal -->
    <button
      :id="'btn-' + modalId"
      type="button"
      class="btn btn-primary"
      data-bs-toggle="modal"
      :data-bs-target="'#' + modalId"
      @click="OpenModal()"
    >
      <slot name="button">
        <i class="bi bi-plus-circle" />
      </slot>
    </button>
    <button
      :id="'btn-' + modalId + '-hide'"
      type="button"
      class="d-none"
      data-bs-toggle="modal"
      :data-bs-target="'#' + modalId"
    >
      <slot name="button">
        <i class="bi bi-plus-circle" />
      </slot>
    </button>
    <div
      :id="modalId"
      class="modal fade"
      :data-bs-backdrop="isStatic"
      data-bs-keyboard="false"
      tabindex="-1"
      :aria-labelledby="modalId + 'Label'"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <slot name="content">
            <div class="modal-header">
              <h5 :id="modalId + 'Label'" class="modal-title">
                Modal title
              </h5>
              <button
                type="button"
                class="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
                @click="CloseModal()"
              />
            </div>
            <div class="modal-body" />
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                data-bs-dismiss="modal"
                @click="CloseModal()"
              >
                Cerrar
              </button>
              <button type="button" class="btn btn-primary">
                Aceptar
              </button>
            </div>
          </slot>
        </div>
      </div>
    </div>
    <!-- <div class="modal-template">
    <div class="modal-open">
      <div
        class="modal"
        style="display: block"
      >
        <div
          class="modal-dialog "
          :class="size"
        >
          <div class="modal-content">
            <slot>
              <div class="modal-header">
                <span class="h4 font-bold"> </span>
              </div>
              <div class="modal-body">
                <span class="h4 font-bold"> </span>
              </div>
            </slot>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade in" />
  </div> -->
  </div>
</template>

<script lang="ts" setup>
defineProps({
  size: {
    type: String,
    default: "lg"
  },
  isStatic: {
    type: Boolean,
    default: false
  },
  modalId: {
    type: String,
    default: ""
  }
});

const emit = defineEmits(["close", "open"]);

const CloseModal = () => {
  emit("close");
};

const OpenModal = () => {
  emit("open");
};
</script>
