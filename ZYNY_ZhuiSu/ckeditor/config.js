/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
   
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	   config.language = 'zh-cn'; //中文 
    config.uiColor = '#eef5fd'; //'#CCEAFE';  //编辑器颜色 
    config.font_names = '宋体;楷体_GB2312;新宋体;黑体;隶书;幼圆;微软雅黑;Arial;Comic Sans MS;Courier New;Tahoma;Times New Roman;Verdana';
    var ckfinderPath ="/ckfinder"; //ckfinder路径 
    config.filebrowserBrowseUrl = ckfinderPath + '/ckfinder.html'; config.filebrowserImageBrowseUrl = ckfinderPath + '/ckfinder.html?type=Images';//下面都是
    config.filebrowserFlashBrowseUrl = ckfinderPath + '/ckfinder.html?type=Flash'; config.filebrowserUploadUrl = ckfinderPath +
    '/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files'; config.filebrowserImageUploadUrl = ckfinderPath +
    '/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = ckfinderPath +
    '/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
};
