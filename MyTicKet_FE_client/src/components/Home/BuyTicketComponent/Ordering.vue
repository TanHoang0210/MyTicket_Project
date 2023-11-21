<template>
    <div>  
        <div>
            <Header :isLogin="!isLogin"></Header>
        </div>
        <LoadPage />
    </div>
</template>
<script>
import LoadPage from "@/views/LoadPage.vue"
import Header from "@/components/Header.vue";
export default {
    name: 'BuyTicketView',
    components: {
        LoadPage,Header
    }
}
</script>