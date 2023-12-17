<template>
  <table class="table">
    <thead>
      <slot name="columns">
        <tr>
          <th v-for="column in columns" :key="column.field">{{ column.label }}</th>
          <th>Thao tác</th>
        </tr>
      </slot>
    </thead>
    <tbody>
      <tr v-for="(item, index) in data" :key="index">
        <slot :row="item">
          <td v-for="column in columns" :key="column.field" v-if="hasValue(item, column.field)">{{ itemValue(item,
            column.field) }}</td>
        </slot>
        <td>
          <router-link class="btn btn-info btn-fill" :style="{ color: '#fff' }"
            :to="{ name: 'EventDetail', query: { id: item.id } }">
            Chi tiết
          </router-link>
        </td>
      </tr>
    </tbody>
  </table>
</template>
<script>
export default {
  name: 'l-table',
  props: {
    columns: Array,
    data: Array,
    action: "",
  },
  methods: {
    hasValue(item, column) {
      return item[column] !== 'undefined'
    },
    itemValue(item, column) {
      return item[column]
    }
  }
}
</script>
<style></style>
