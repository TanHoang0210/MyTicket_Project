<template>
    <div class="aboutEvent">
        <div class="container">
            <div class="venue-container">
                <div v-if="isInfo">
                    <venue-info :id="venueInfo" :currentVenue="currentVenue"></venue-info>
                </div>
                <div v-else>
                    <main-Event :id="eventVenue" :events="currentVenue.eventVenues"></main-Event>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import VenueInfo from '@/components/Home/VenueDetailComponent/AboutVenueComponent/VenueInfo.vue'
import MainEvent from "@/components/Home/MainComponent/EventsComponent.vue/MainEvents.vue"
export default {
    name: 'EventDetailView',
    props: ['currentVenue', 'isInfo'],
    components: {
        VenueInfo, MainEvent
    }
}
</script>
<style>
.venue-container {
    margin: 50px 0;
}
</style>