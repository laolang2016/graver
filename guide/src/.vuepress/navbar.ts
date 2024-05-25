import { navbar } from "vuepress-theme-hope";

export default navbar([
  "/",
  {
    text: "起步与环境配置",
    icon: "lightbulb",
    prefix: "/setup/",
    children: [
      {
        text: "vscode",
        icon: "lightbulb",
        prefix: "vscode/",
        children: ["a01_reference.md","a02_base_setting.md"],
      },
      {
        text: "cmake",
        icon: "lightbulb",
        prefix: "cmake/",
        children: ["a01_reference.md","a02_hello.md"],
      },
    ],
  },
  "/alex/",
]);
