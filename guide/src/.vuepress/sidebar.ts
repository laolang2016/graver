import { sidebar } from "vuepress-theme-hope";

export default sidebar({
  "/": [
    "",
    "portfolio",
    {
      text: "起步与环境配置",
      icon: "laptop-code",
      prefix: "setup/",
      link: "setup/",
      // collapsible: true,
      children: [
        {
          text: "vscode",
          prefix: "vscode/",
          link: "vscode/",
          collapsible: true,
          children: "structure",
        },
        {
          text: "cmake",
          prefix: "cmake/",
          link: "cmake/",
          collapsible: true,
          children: "structure",
        },
      ],
    },
    {
      text: "alex",
      icon: "laptop-code",
      prefix: "alex/",
      link: "alex/",
      // collapsible: true,
      children: "structure",
    }
  ],
});
