<script>
import { MinusIcon, MaximizeIcon, MinimizeIcon, XIcon } from "lucide-vue-next";
export default {
  name: "TitleBar",
  components: {
    MinusIcon,
    MaximizeIcon,
    MinimizeIcon,
    XIcon,
  },
  data() {
    return {
      isMaximized: false,
    };
  },
  mounted() {
    // Listen for window state changes
    window.electronAPI.onWindowState((state) => {
      this.isMaximized = state === "maximized";
    });

    // Get initial state
    window.electronAPI.isMaximized().then((maximized) => {
      this.isMaximized = maximized;
    });
  },
  methods: {
    minimize() {
      window.electronAPI.minimize();
    },
    toggleMaximize() {
      window.electronAPI.toggleMaximize();
    },
    close() {
      window.electronAPI.close();
    },
  },
};
</script>

<template>
  <div
    class="title-bar flex justify-between items-center relative bg-gray-800"
    id="title-bar"
  >
    <div
      class="title-bar__center absolute inset-x-0 text-center z-0 items-center sm:flex justify-center hidden"
    >
      <slot name="center">
        <span class="text-white">ZWorkshop</span>
      </slot>
    </div>
    <div class="title-bar__left z-10">
      <slot name="left"></slot>
    </div>

    <div class="title-bar__right flex z-10">
      <slot name="right">
        <button
          @click="minimize"
          class="w-10 h-8 flex items-center justify-center text-white hover:bg-gray-500"
          tabindex="-1"
        >
          <MinusIcon :size="18" />
        </button>
        <button
          @click="toggleMaximize"
          class="w-10 h-8 flex items-center justify-center text-white hover:bg-gray-500"
          tabindex="-1"
        >
          <!-- <component
            :is="isMaximized ? RestoreIcon : MaximizeIcon"
            :size="16"
          /> -->
          <MinimizeIcon v-if="isMaximized" :size="18" />
          <MaximizeIcon v-else :size="16" />
        </button>
        <button
          @click="close"
          class="w-10 h-8  flex items-center justify-center text-white hover:bg-red-600"
          tabindex="-1"
        >
          <XIcon :size="18" />
        </button>
      </slot>
    </div>
  </div>
</template>

<style scoped>
#title-bar {
  -webkit-app-region: drag;
}
.title-bar__right {
  -webkit-app-region: no-drag;
}
</style>
