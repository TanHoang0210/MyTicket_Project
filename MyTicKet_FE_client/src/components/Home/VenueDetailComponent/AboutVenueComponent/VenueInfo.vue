<template>
    <section id="eventRefund" class="box-shadow pb-4 pt-3 section-content">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-24">

                    <h1 class="black-heading">
                        Thông tin sân vận động
                    </h1>
                    <div id="eventfaq-content">
                        <Strong>Địa chỉ: </Strong> {{ currentVenue.address }}
                        <br>
                        <Strong>Sức chứa: </Strong> {{ currentVenue.capacity }}
                        <br>
                        <span>{{ currentVenue.description }}</span>
                    </div>
                </div>
        </div>
    </div>
</section>
</template>
<script>
    export default {
  props:['currentVenue'],
}
</script>
<style>
#eventfaq-content{
    font-size: 1.3rem;
}
</style>