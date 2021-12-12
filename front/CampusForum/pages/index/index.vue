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
	
	export default {
		data() {
			return {
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
			
		},
		methods: {

		}
	}
</script>

<style>
</style>
