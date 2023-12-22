<template>
    <div class="aboutEvent">
        <ticket-list v-show="isShowList" :currentEvent="currentEvent" :listTickets="listTickets"></ticket-list>
        <event-detail :currentEvent="currentEvent"></event-detail>
        <ticket-price :currentEvent="currentEvent"></ticket-price>
        <event-refund :currentEvent="currentEvent"></event-refund>
        <event-policy :currentEvent="currentEvent"></event-policy>
    </div>
</template>
<script>
import TicketList from '@/components/Home/EventDetailComponent/AboutEventComponent/TicketList.vue'
import EventDetail from '@/components/Home/EventDetailComponent/AboutEventComponent/EventDetail.vue'
import TicketPrice from '@/components/Home/EventDetailComponent/AboutEventComponent/TicketPrice.vue'
import EventRefund from '@/components/Home/EventDetailComponent/AboutEventComponent/EventRefund.vue'
import EventPolicy from '@/components/Home/EventDetailComponent/AboutEventComponent/EventPolicy.vue'
export default {
  name: 'EventDetailView',
  props:['currentEvent','listTickets','isShowList'],
  components:{
    TicketList,EventDetail,TicketPrice,EventRefund,EventPolicy
  },
  mounted(){
    console.log(this.show + "haha")
  }
}
</script>
<style>
</style>