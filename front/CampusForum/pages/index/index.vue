<template>
	<view>
		<view v-for="(item,index) in stateList">
			<view>
				<text>{{ item.title }}</text>
			</view>
			<view>
				<text>{{ item.userId }}</text>
			</view>
		</view>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as stateApi from "../../api/state.js"
	import * as config from "../../utils/config.js"
	
	export default {
		data() {
			return {
				token: null,
				page: 1,
				stateList: [
					{
						title: '测试状态',
						userId: 1, // 用户id
						context: '测试的正文',
						share: true
					}
				]
			}
		},
		onLoad() {
			this.token = config.getToken()
			if (this.token === null) {
				this.login()
			} else {
				// 获取状态列表
				// this.stateList = stateApi.getStateById(this.token, this.page) 
			}
		},
		methods: {
			login() {
				// 跳转到登陆页面
			}
		}
	}
</script>

<style>
</style>
