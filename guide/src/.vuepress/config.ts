import { defineUserConfig } from "vuepress";
import theme from "./theme.js";

export default defineUserConfig({
  base: "/",

  lang: "zh-CN",
  title: "graver",
  description: "编译原理学习指南",

  markdown:{
    headers:{
      level:[2,3,4,5,6]
    }
  },

  theme,

  // 和 PWA 一起启用
  // shouldPrefetch: false,
});
