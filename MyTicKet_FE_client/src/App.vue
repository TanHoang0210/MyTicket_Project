<template>
  <!DOCTYPE html>
  <html lang="en">
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
  </head>
  <body>
    <div id="app">
      <router-view/>
  </div>
  </body>
  </html>
</template>
<script>

</script>
<style>
body{
    margin: 0px;
    font-family: "Candara" !important;
    font-weight: normal;
    min-height: 100%;
    scroll-behavior: smooth;
    box-sizing: border-box;
}
#app{
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background: #fff;
}
:root {
    --primary-color: #FDF9F7;
    --primary-color-bold:rgb(226, 168, 140);
    --primary-color--hover: rgb(0,0,0,0.1);
    --text-color: #555;
    --carousel-width: 880px;
    --primary-color-hover-bold:rgb(188, 101, 60);
}
</style>
