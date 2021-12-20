<template>
	<view>
		<view class="uni-title" style="font-size:20px;padding:15rpx;">标题</view>
		<view class="uni-textarea">
			<textarea auto-height style="border-style:solid;border-width:thin;width: 100%;height: 25px;" />
		</view>
		<view class="uni-title" style="font-size:20px;padding:15rpx;">正文</view>
		<view class="uni-textarea">
			<textarea style="border-style:solid;border-width:thin;width: 100%;height: 200px;" />
		</view>
		<button class="bottom" @click="sendstate()">发送状态</button>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				
			}
		},
		methods: {
			sendstate(){
				console.log('发送了当前页面')
			}
		}
	}
</script>

<style>
	.bottom{
		width: 90%;
		margin: 30rpx 5%;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
		background-image: linear-gradient(120deg, #a1c4fd 0%, #c2e9fb 100%);
	}
</style>
