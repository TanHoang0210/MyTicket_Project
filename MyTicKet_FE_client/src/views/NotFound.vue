<template>
    <div class="notfound">
        <div style="margin: auto;">
            <img :src=$fileUrl+notFoundImg alt="">
            <h1 style="text-align: center; font-family: Candara; font-weight: 600; color: #a485f2;">404 PAGE NOT FOUND</h1>
        </div>
    </div>

</template>

<script>
export default{
    name:"PageNotFound",
    data(){
        return{
            notFoundImg:"api/file/get?folder=notfound&file=notfound.webp"
        }
    }
}
</script>

<style>
.notfound{
    display: flex;
    position: relative;
    width: 100vw;
    height: 100vh;

}

</style>