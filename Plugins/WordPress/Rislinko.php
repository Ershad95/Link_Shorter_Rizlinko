<?php

 /**
 * Plugin Name:       Rislinko
 * Plugin URI:        https://example.com/plugins/the-basics/
 * Description:       With Rizlinko you can generate short link from long links!!
 * Version:           1
 * Requires at least: 5.2
 * Requires PHP:      7.2
 * Author:            Ershad Raoufi
 * License:           GPL v2 or later
 * License URI:       https://www.gnu.org/licenses/gpl-2.0.html
 * Update URI:        https://rlnk.ir/
 * Text Domain:       rlnk.ir
 */
  //-----Add Rislinko Item to Menu----------
  add_action('admin_menu', 'rislinkoMenu');
 
function rislinkoMenu(){
    add_menu_page( 'ریزلینکو', 
    'ریزلینکو', 'manage_options', 
    'ریزلینکو', 
    'wporg_options_page_html');
}
?>
<?php
    function wporg_options_page_html() {
    ?>
    <link href="<?php echo plugin_dir_url(__FILE__) ?>/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="<?php echo plugin_dir_url(__FILE__) ?>/Rislinko.php.css" rel="stylesheet" />
       <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="center-logo">
                    <img src="<?php echo plugin_dir_url(__FILE__) ?>/icons/mipmap-hdpi/ic_launcher.png" />
                    <h4 style="line-height:2.5">ریزلینکو</h4>
                </div>
                <div class="input-group">
                    <input id="OrginalUrl" style="text-align:left" placeholder="لینک بلند خود را وارد کنید" class="form-control " />
                    <span class="input-group-text" id="basic-addon1">.http://www</span>
                </div>
                <div id="error">
                 
                </div>
                <div class="input-group">
                    <span class="input-group-text" id="basic-addon1">پسوند لینک (اختیاری)</span>
                    <input id="ShortUrl" placeholder="پسوند لینک را مشخص کنید" class="form-control " />
                </div>
                <div class="input-group">
                    <span class="input-group-text" id="basic-addon2">رمزعبور لینک (اختیاری)</span>
                    <input id="Password" placeholder="رمزعبور لینک را مشخص کنید" class="form-control " />
                </div>
                <button onclick="" style="margin:5px;" class="btn btn-success">تبدیل لینک</button>
            </div>
        </div>

    </div>
    <script src="<?php echo plugin_dir_url(__FILE__) ?>/Rislinko.php.js"></script>
    <?php
}
?>