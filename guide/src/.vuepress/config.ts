import { defineUserConfig } from "vuepress";
import theme from "./theme.js";

export default defineUserConfig({
  base: "/",

  lang: "zh-CN",
  title: "graver",
  description: "编译原理学习指南",

  theme,

  // 和 PWA 一起启用
  // shouldPrefetch: false,
});
