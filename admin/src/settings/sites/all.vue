<template>
  <div class="app-container">

    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>{{ $t('route.settingsSitesAll') }}</span>
        <el-button style="float: right; padding: 3px 0" type="text">操作按钮</el-button>
      </div>
      <el-table
        :data="tableData"
        stripe
        style="width: 100%"
      >
        <el-table-column
          prop="name"
          :label="$t('name')"
          width="220"
        />
        <el-table-column
          prop="value"
          :label="$t('value')"
        />
      </el-table>
    </el-card>

  </div>
</template>

<script>
import { getInfo } from '@/api/cms'

export default {
  name: 'Info',
  data() {
    return {
      tableData: []
    }
  },
  created() {
    this.getInfo()
  },
  methods: {
    getInfo() {
      getInfo().then(response => {
        this.tableData.push({
          name: this.$t('tools.info.apiUrl'),
          value: response.apiUrl
        })
        this.tableData.push({
          name: this.$t('tools.info.apiServerName'),
          value: response.apiServerName
        })
        this.tableData.push({
          name: this.$t('tools.info.contentRootPath'),
          value: response.contentRootPath
        })
        this.tableData.push({
          name: this.$t('tools.info.webRootPath'),
          value: response.webRootPath
        })
        this.tableData.push({
          name: this.$t('tools.info.remoteIpAddress'),
          value: response.remoteIpAddress
        })
        this.tableData.push({
          name: this.$t('tools.info.targetFramework'),
          value: response.targetFramework
        })
        this.tableData.push({
          name: this.$t('tools.info.productVersion'),
          value: response.productVersion
        })
        this.tableData.push({
          name: this.$t('tools.info.pluginVersion'),
          value: response.pluginVersion
        })
        this.tableData.push({
          name: this.$t('tools.info.databaseType'),
          value: response.databaseType
        })
        this.tableData.push({
          name: this.$t('tools.info.database'),
          value: response.database
        })
        this.tableData.push({
          name: this.$t('tools.info.cacheType'),
          value: response.cacheType
        })
      })
    }
  }
}
</script>
